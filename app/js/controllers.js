'use strict';

/* Controllers */

angular.module('myApp.controllers', ['ngRoute', 'myApp.services']).
  controller('HomeController', [
    function () {
      console.log('HomeController');
    }
  ]).
  controller('EmployeesController', ['$scope', '$routeParams', '$location', 'northwindService',
    function ($scope, $routeParams, $location, northwindService) {

      $scope.addNewEmployee = function () {
        $location.path('/employees/add');
        return;
      };

      $scope.editSelectedEmployee = function () {
        $location.path('/employees/edit');
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
        $location.path('/employees/view');
        return;
      };

      if ($routeParams.loadVal == 'true') {
        $scope.northwind.getAllEmployees();
      }
    }
  ]).
  controller('AddEmployeeController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
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
  controller('EditEmployeeController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
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
  controller('ViewEmployeeController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
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
