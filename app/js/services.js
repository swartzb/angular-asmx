'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('myApp.services', []).
  value('version', '0.1').
  factory('northwindService', ['$http', function ($http) {
    var serviceInstance = {
      employees: [],

      errorInfo: {},

      status: undefined,

      httpState: 'idle',

      getAllEmployees: function () {
        var that = this;
        this.employees = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetAllEmployees',
          method: "POST",
          data: "{}",
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.success(function (data, status, headers, config) {
          that.employees = data.d;
          that.status = status;
          that.httpState = 'success';
          return;
        }).error(function (data, status, headers, config) {
          that.errorInfo = data;
          that.status = status;
          that.httpState = 'error';
          return;
        });

        return firstPromise;
      }
    };

    return serviceInstance;
  }]);
