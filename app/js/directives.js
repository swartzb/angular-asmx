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
            if (val) {
              var s = val.slice(6, -2);
              var i = parseInt(s);
              var d = new Date(i);
              var outVal = d.toLocaleDateString("en-US");
              return outVal;
            } else {
              return val;
            }
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
  ]);
