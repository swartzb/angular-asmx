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
              var outVal = d.getMonth() + '/' + d.getDate() + '/' + d.getFullYear();
              return outVal;
            } else {
              return val;
            }
          });
          ctrl.$parsers.unshift(function (viewValue) {
            if (!viewValue) {
              // empty is valid
              ctrl.$setValidity('validDate', true);
              return viewValue;
            } else {
              var testDate = Date.parse(viewValue);
              if (!Number.isNaN(testDate)) {
                // it is valid
                ctrl.$setValidity('validDate', true);
                return viewValue;
              } else {
                // it is invalid, return undefined (no model update)
                ctrl.$setValidity('validDate', false);
                return undefined;
              }
            }
          });
        }
      };
    }
  ]);
