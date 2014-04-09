'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('myApp.services', []).
  value('version', '0.1').
  factory('northwindService', ['$http', function ($http) {
    var serviceInstance = {
      getAllEmployees: function () {
        var promise = $http({
          url: '../NwndSvc.asmx/GetAllEmployees',
          method: "POST",
          data: "{}",
          headers: { 'Content-Type': 'application/json' }
        });
        return promise;
      }
    };

    return serviceInstance;
  }]);
