'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('myApp.services', []).
  value('version', '0.1').
  factory('northwindService', ['$http', function ($http) {
    var employees, errorInfo;

    var serviceInstance = {
      getAllEmployees: function () {
        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetAllEmployees',
          method: "POST",
          data: "{}",
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.success(function (data, status, headers, config) {
          employees = data.d;
          return;
        }).error(function (data, status, headers, config) {
          errorInfo = data;
        });

        return firstPromise;
      }
    };

    return serviceInstance;
  }]);
