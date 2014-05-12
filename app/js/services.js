'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('myApp.services', []).
  value('version', '0.1').
  factory('northwindService', ['$http', function ($http) {
    var serviceInstance = {
      selectedEmployee: null,

      employees: [],

      errorInfo: {},

      status: undefined,

      httpState: 'idle',

      getAllEmployees: function () {
        var that = this;
        this.selectedEmployee = null;
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
      },

      deleteEmployee: function (empl) {
        var that = this;
        var inData = {
          id: empl.EmployeeID
        };

        this.selectedEmployee = null;
        this.employees = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/DeleteEmployee',
          method: "POST",
          data: JSON.stringify(inData),
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
      },

      addEmployee: function (empl) {
        var that = this;
        var inData = {
          employee: empl
        };

        this.selectedEmployee = null;
        this.employees = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/AddEmployee',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.success(function (data, status, headers, config) {
          that.employees = data.d.employees;
          for (var i = 0, len = that.employees.length, id = data.d.id; i < len; ++i) {
            if (id == that.employees[i].EmployeeID) {
              that.selectedEmployee = that.employees[i];
              break;
            }
          }
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
