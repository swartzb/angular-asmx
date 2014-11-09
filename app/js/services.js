'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('myApp.services', []).
  factory('northwindService', ['$http', function ($http) {
    var serviceInstance = {
      selectedEmployee: null,

      details: {},

      employee: null,

      employees: [],

      territories: [],

      errorInfo: {},

      status: undefined,

      httpState: 'idle',

      toggleSelect: function (employee) {
        this.selectedEmployee = (employee == this.selectedEmployee) ? null : employee;
        return;
      },

      getDisplayDate: function (val) {
        var re = /^\/Date\((-?\d+)\)\/$/;
        var result = re.exec(val);
        if (!result || result.length < 2) {
          return val;
        }

        var i = parseInt(result[1]);
        var d = new Date(i);
        if (Number.isNaN(d)) {
          return val;
        }

        var outVal = d.toLocaleDateString("en-US");
        return outVal;
      },

      getEmployees: function () {
        var that = this;
        this.selectedEmployee = null;
        this.employees = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetEmployees',
          method: "POST",
          data: "{}",
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            var i, len;

            that.employees = data.d;
            for (i = 0, len = that.employees.length; i < len; ++i) {
              that.employees[i].selectedTerritory = that.employees[i].TerritoryNames[0];
            }

            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
            that.errorInfo = data;
            that.status = status;
            that.httpState = 'error';
            return;
          });

        return secondPromise;
      },

      updateTerritories: function(empId, territoryIDs) {
        var that = this;
        var inData = {
          id: empId,
          territoryIDs: territoryIDs
        };

        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/UpdateTerritories',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            var i, len;

            that.employees = data.d;
            for (i = 0, len = that.employees.length; i < len; ++i) {
              if (that.employees[i].TerritoryNames.length > 0) {
                that.employees[i].selectedTerritory = that.employees[i].TerritoryNames[0];
              }
            }
            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
            that.errorInfo = data;
            that.status = status;
            that.httpState = 'error';
            return;
          });

        return secondPromise;
      },

      getCanReportTo: function (empId) {
        var that = this;
        var inData = {
          id: empId
        };

        this.canReportTo = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetCanReportTo',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            that.canReportTo = data.d;
            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
            that.errorInfo = data;
            that.status = status;
            that.httpState = 'error';
            return;
          });

        return secondPromise;
      },

      getTerritories: function (empId) {
        var that = this;
        var inData = {
          id: empId
        };

        this.territories = [];
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetTerritories',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            that.territories = data.d;
            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
            that.errorInfo = data;
            that.status = status;
            that.httpState = 'error';
            return;
          });

        return secondPromise;
      },

      getEmployeeDetails: function (empId, newEmp) {
        var that = this;
        var inData = {
          id: empId,
          newEmployee: newEmp
        };

        this.employee = null;
        this.errorInfo = {};
        this.status = undefined;
        this.httpState = 'inProgress';

        var firstPromise = $http({
          url: '../NwndSvc.asmx/GetEmployeeDetails',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            that.details = data.d;
            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
            that.errorInfo = data;
            that.status = status;
            that.httpState = 'error';
            return;
          });

        return secondPromise;
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
          var i, len;

          that.employees = data.d;
          for (i = 0, len = that.employees.length; i < len; ++i) {
            that.employees[i].selectedTerritory = that.employees[i].TerritoryNames[0];
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
      },

      editEmployee: function (empl) {
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
          url: '../NwndSvc.asmx/EditEmployee',
          method: "POST",
          data: JSON.stringify(inData),
          headers: { 'Content-Type': 'application/json' }
        });

        var secondPromise = firstPromise.
          success(function (data, status, headers, config) {
            that.employees = data.d.employees;
            for (var i = 0, len = that.employees.length, id = empl.EmployeeID; i < len; ++i) {
              if (id == that.employees[i].EmployeeID) {
                that.selectedEmployee = that.employees[i];
                break;
              }
            }
            that.status = status;
            that.httpState = 'success';
            return;
          }).
          error(function (data, status, headers, config) {
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
