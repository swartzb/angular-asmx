'use strict';

/* Directives */


angular.module('myApp.directives', ['myApp.services']).
  directive('appVersion', ['version',
    function (version) {
      return function (scope, elm, attrs) {
        elm.text(version);
      };
    }
  ]).
  directive('validDate', [
    function () {
      return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
          ctrl.$formatters.unshift(function (val) {
            var re = /-?\d+/;
            var result = re.exec(val);
            if (!result || result.length == 0) {
              return val;
            }

            var i = parseInt(result[0]);
            var d = new Date(i);
            if (Number.isNaN(d)) {
              return val;
            }

            var outVal = d.toLocaleDateString("en-US");
            return outVal;
          });
          ctrl.$parsers.unshift(function (viewValue) {
            ctrl.$setValidity('validDate', true);
            ctrl.$setValidity('futureDate', true);
            ctrl.$setValidity('fullYear', true);

            // check for valid date
            if (!viewValue) {
              // empty is valid
              return viewValue;
            }

            var testDate = Date.parse(viewValue);
            if (Number.isNaN(testDate)) {
              // date invalid, return undefined (no model update)
              ctrl.$setValidity('validDate', false);
              return undefined;
            }

            // date is valid

            // check for 4 digit year
            var re = /\D\d{4}$/;
            if (!re.test(viewValue)) {
              ctrl.$setValidity('fullYear', false);
              return undefined;
            }

            // date has 4-digit year

            // check that date is not in future
            var hireDate = new Date(viewValue);
            var rightNow = new Date();
            var today = new Date(rightNow.toDateString());
            if (hireDate > today) {
              ctrl.$setValidity('futureDate', false);
              return undefined;
            }

            // date not in future
            return viewValue;
          });
        }
      };
    }
  ]).
  directive('employeesList', ['$window', '$timeout', 'northwindService',
    function ($window, $timeout, northwindService) {
      var timeoutPromise = null;
      var ulResizer = function () {
        var windowInnerHeight = $window.innerHeight;
        var ul = document.getElementById('vertScrollList');
        if (ul) {
          var ulTop = ul.offsetTop;
          var ulBottomMargin = parseInt(window.getComputedStyle(ul, null).getPropertyValue("margin-bottom"));
          var newHeight = windowInnerHeight - ulTop - ulBottomMargin;
          if (newHeight > 400) {
            ul.style.height = newHeight + 'px';
            console.log(windowInnerHeight + ',' + ulTop + ',' + newHeight + ',' + ulBottomMargin);
          } else {
            ul.style.height = null;
            console.log('removed height');
          }
        }
        timeoutPromise = null;
      };
      var directiveObject = {
        templateUrl: 'partials/employeesList.html',
        //scope: {
        //  northwind: '=employeesList'
        //},
        link: function (scope, element, attrs) {
          $window.addEventListener('resize',
            function () {
              if (!timeoutPromise) {
                timeoutPromise = $timeout(ulResizer, 500);
              }
            }
          );
          scope.$on('$destroy', function () {
            $window.removeEventListener('resize', ulResizer);
          });
          scope.$watchCollection('northwind.employees',
            function (newVal, oldVal) {
              //if (newVal.length === oldVal.length) {
              //  return;
              //}

              /* Wrapping the function inside a $timeout will ensure that
               * the code is executed after the next browser rendering
               * (thus after the modified list has been processed by ngRepeat) */
              $timeout(ulResizer);
            }
          );
        }
      };
      return directiveObject;
    }
  ]);
