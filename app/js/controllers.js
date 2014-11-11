'use strict';

/* Controllers */

angular.module('myApp.controllers', ['ngRoute', 'myApp.services']).
  controller('CanReportToController', ['$scope', '$routeParams',
    function ($scope, $routeParams) {
      var i, len;

      console.log('CanReportToController');

      $scope.routeParams = $routeParams;

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

      $scope.addNewEmployee = function () {
        $scope.location.path('/employees/add');
        return;
      };

      $scope.editSelectedEmployee = function () {
        $scope.location.path('/employees/edit');
        return;
      };

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
  controller('AddEmployeeController', ['$scope', '$location',
    function ($scope, $location) {
      $scope.readOnly = false;
      $scope.employee = {};
      $scope.headerText = 'Add New Employee';

      $scope.getCssClasses = function (ngModelContoller) {
        var classes = {};
        switch (ngModelContoller.$name) {
          case 'lName':
          case 'fName':
          case 'hDate':
            classes = {
              'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
              'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
            };
            break;
          default:
            classes = {
              'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
              'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
            };
            break;
        }
        return classes;
      };

      $scope.okButtonText = 'Add';
      $scope.isOkButtonDisabled = function (FormController) {
        return FormController.$pristine || FormController.$invalid;
      };

      $scope.doCancel = function () {
        $location.path('/employees/load/false');
      };

      $scope.doOK = function () {
        $scope.northwind.addEmployee($scope.employee).
          success(function (data, status, headers, config, statusText) {
            $location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
      };

      $scope.northwind.getEmployeeDetails(0, true);
    }
  ]).
  controller('IndexController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
      $scope.northwind = northwindService;
      $scope.location = $location;
    }
  ]).
  controller('EditEmployeeController', ['$scope', '$location',
    function ($scope, $location) {
      $scope.readOnly = false;
      $scope.employee = angular.copy($scope.northwind.selectedEmployee);
      $scope.headerText = 'Edit ' + $scope.northwind.selectedEmployee.DisplayName;

      $scope.getCssClasses = function (ngModelContoller) {
        var classes = {};
        if (ngModelContoller) {
          switch (ngModelContoller.$name) {
            case 'lName':
            case 'fName':
            case 'hDate':
              classes = {
                'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
                'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
              };
              break;
            default:
              classes = {
                'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
                'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
              };
              break;
          }
        }
        return classes;
      };

      $scope.okButtonText = 'Update';
      $scope.isOkButtonDisabled = function (FormController) {
        return FormController.$pristine || FormController.$invalid;
      };

      $scope.doCancel = function () {
        $location.path('/employees/load/false');
      };

      $scope.doOK = function () {
        $scope.northwind.editEmployee($scope.employee).
          success(function (data, status, headers, config, statusText) {
            $location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {

          });
      };

      var myPromise = $scope.northwind.getEmployeeDetails($scope.employee.EmployeeID, false);
      myPromise.success(function (data, status, headers, config) {
        if ($scope.employee.ReportsTo) {
          for (var k = 0, len2 = $scope.northwind.details.canReportTo.length; k < len2; ++k) {
            if ($scope.northwind.details.canReportTo[k].EmployeeID == $scope.employee.ReportsTo) {
              $scope.employee.Supervisor = $scope.northwind.details.canReportTo[k];
              break;
            }
          }
        }
      });
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
      $scope.isOkButtonDisabled = function (FormController) {
        return false;
      };
      $scope.doCancel = function () {
        $location.path('/employees/load/false');
      };
      $scope.doOK = function () {
        $location.path('/employees/load/false');
      };
    }
  ]);
