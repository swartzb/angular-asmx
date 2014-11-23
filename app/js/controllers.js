'use strict';

/* Controllers */

angular.module('myApp.controllers', ['ngRoute', 'myApp.services']).
  controller('CanReportToController', ['$scope', '$routeParams', '$window',
    function ($scope, $routeParams, $window) {
      var i, len;

      console.log('CanReportToController');

      $scope.routeParams = $routeParams;

      $scope.$watch(
        function () {
          return $window.document.getElementById("noSup");
        },
        function (newValue, oldValue) {
          if (newValue && !oldValue) {
            if ($scope.routeParams.supId == 'null') {
              $window.document.getElementById('noSup').checked = true;
            }
          }
        });

      $scope.doOK = function () {
        $scope.northwind.updateSupervisor($scope.routeParams.empId, $scope.routeParams.supId).
          success(function (data, status, headers, config, statusText) {
            $scope.location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
        return;
      };

      for (i = 0, len = $scope.northwind.employees.length; i < len; ++i) {
        if ($scope.northwind.employees[i].EmployeeID == $scope.routeParams.empId) {
          $scope.employeeName = $scope.northwind.employees[i].Name;
        }
      }

      $scope.northwind.getCanReportTo($scope.routeParams.empId);
    }
  ]).
  controller('TerritoriesForEmployeeController', ['$scope', '$routeParams',
    function ($scope, $routeParams) {
      var i, len;

      console.log('TerritoriesForEmployeeController');

      $scope.doOK = function () {
        var territoryIDs = [];
        for (i = 0, len = $scope.northwind.territories.length; i < len; ++i) {
          if ($scope.northwind.territories[i].EmployeeCoversTerritory) {
            territoryIDs.push($scope.northwind.territories[i].TerritoryID);
          }
        }
        $scope.northwind.updateTerritories($scope.id, territoryIDs).
          success(function (data, status, headers, config, statusText) {
            $scope.location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
        return;
      };

      $scope.id = $routeParams.id;

      for (i = 0, len = $scope.northwind.employees.length; i < len; ++i) {
        if ($scope.northwind.employees[i].EmployeeID == $scope.id) {
          $scope.employeeName = $scope.northwind.employees[i].Name;
        }
      }

      $scope.northwind.getTerritories($scope.id);
    }
  ]).
  controller('HomeController', [
    function () {
      console.log('HomeController');
    }
  ]).
  controller('EmployeesController', ['$scope', '$routeParams',
    function ($scope, $routeParams) {
      console.log('EmployeesController');

      $scope.deleteEmployee = function (employee) {
        $scope.northwind.deleteEmployee(employee).
          success(function (data, status, headers, config, statusText) {

          }).
          error(function (data, status, headers, config, statusText) {

          });
        return;
      };

      $scope.viewSelectedEmployeeDetails = function () {
        $scope.location.path('/employees/view');
        return;
      };

      if ($routeParams.loadVal == 'true') {
        $scope.northwind.getEmployees();
      }
    }
  ]).
  controller('AddEmployeeController', ['$scope',
    function ($scope) {

      console.log('AddEmployeeController');

      $scope.readOnly = false;
      $scope.headerText = 'Add';

      $scope.okButtonText = 'Add';

      $scope.doOK = function () {
        var d1 = $scope.northwind.employee.HireDate;
        var d2 = $scope.getDisplayDate(d1);
        $scope.northwind.employee.HireDate = d2;

        $scope.northwind.addEmployee($scope.northwind.employee).
          success(function (data, status, headers, config, statusText) {
            $scope.location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
      };

      $scope.northwind.getEmployee(0, true);
    }
  ]).
  controller('IndexController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
      console.log('IndexController');

      $scope.northwind = northwindService;
      $scope.location = $location;

      $scope.getDisplayDate = function (val) {
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

        var fy = d.getUTCFullYear();
        var mo = d.getUTCMonth() + 1;
        var day = d.getUTCDate();
        var outVal = mo + '/' + day + '/' + fy;
        //var outVal = d.toLocaleDateString("en-US");
        return outVal;
      };
    }
  ]).
  controller('EditEmployeeController', ['$scope', '$routeParams',
    function ($scope, $routeParams) {

      console.log('EditEmployeeController');

      $scope.routeParams = $routeParams;

      $scope.readOnly = false;
      $scope.headerText = 'Edit';

      $scope.getCssClasses = function (ngModelContoller) {
        var classes = {
          'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
          'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
        };
        return classes;
      };

      $scope.okButtonText = 'Update';

      $scope.doOK = function () {
        var d1 = $scope.northwind.employee.HireDate;
        var d2 = $scope.getDisplayDate(d1);
        $scope.northwind.employee.HireDate = d2;

        $scope.northwind.editEmployee($scope.northwind.employee).
          success(function (data, status, headers, config, statusText) {
            $scope.location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
      };

      $scope.northwind.getEmployee($scope.routeParams.id, false);
    }
  ]).
  controller('ViewEmployeeController', ['$scope', '$location',
    function ($scope, $location) {
      $scope.readOnly = true;
      $scope.employee = angular.copy($scope.northwind.selectedEmployee);
      $scope.headerText = 'View ' + $scope.northwind.selectedEmployee.DisplayName;
      $scope.getCssClasses = function (ngModelContoller) {
        return {
          'has-success': true
        };
      };
      $scope.okButtonText = 'Ok';

      $scope.doOK = function () {
        $location.path('/employees/load/false');
      };
    }
  ]);
