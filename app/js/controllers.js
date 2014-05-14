'use strict';

/* Controllers */

angular.module('myApp.controllers', ['ngRoute', 'myApp.services']).
  controller('MyCtrl1', [
    function () {
    }
  ]).
  controller('MyCtrl2', [
    function () {
    }
  ]).
  controller('EmployeesController', ['$scope', '$routeParams', '$location', 'northwindService',
    function ($scope, $routeParams, $location, northwindService) {
      $scope.northwind = northwindService;

      $scope.addNewEmployee = function () {
        $scope.setMainMenuEnabled(false);
        $location.path('/employees/add');
        return;
      };

      $scope.editSelectedEmployee = function () {
        $scope.setMainMenuEnabled(false);
        $location.path('/employees/edit');
        return;
      };

      $scope.deleteSelectedEmployee = function () {
        if ($scope.northwind.selectedEmployee) {
          $scope.setMainMenuEnabled(false);
          $scope.northwind.deleteEmployee($scope.northwind.selectedEmployee).
            success(function (data, status, headers, config, statusText) {
              $scope.setMainMenuEnabled(true);
            }).
            error(function (data, status, headers, config, statusText) {
              $scope.setMainMenuEnabled(true);
            });
        }
        return;
      };

      $scope.viewSelectedEmployeeDetails = function () {
        $scope.setMainMenuEnabled(false);
        $location.path('/employees/view');
        return;
      };

      $scope.toggleSelect = function (employee) {
        $scope.northwind.selectedEmployee = (employee == $scope.northwind.selectedEmployee) ? null : employee;
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
      $scope.northwind = northwindService;
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
              'has-success': true
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
        $scope.setMainMenuEnabled(true);
        $location.path('/employees/load/false');
      };
      $scope.doOK = function () {
        $scope.northwind.addEmployee($scope.employee).
          success(function (data, status, headers, config, statusText) {
            $scope.setMainMenuEnabled(true);
            $location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {
            $scope.setMainMenuEnabled(true);
          });
      };
    }
  ]).
  controller('IndexController', ['$scope', '$location',
    function ($scope, $location) {
      $scope.isMainMenuEnabled = true;
      $scope.setMainMenuEnabled = function (enabled) {
        $scope.isMainMenuEnabled = enabled;
      };
      $scope.employeesClickHandler = function () {
        $location.path('/employees/load/true');
      };
    }
  ]).
  controller('EditEmployeeController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
      $scope.readOnly = false;
      $scope.northwind = northwindService;
      $scope.employee = angular.copy($scope.northwind.selectedEmployee);
      $scope.headerText = 'Edit ' + $scope.northwind.getDisplayName($scope.northwind.selectedEmployee);
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
              'has-success': true
            };
            break;
        }
        return classes;
      };
      $scope.okButtonText = 'Update';
      $scope.isOkButtonDisabled = function (FormController) {
        return FormController.$pristine || FormController.$invalid;
      };
      $scope.doCancel = function () {
        $scope.setMainMenuEnabled(true);
        $location.path('/employees/load/false');
      };
      $scope.doOK = function () {
        $scope.northwind.editEmployee($scope.employee).
          success(function (data, status, headers, config, statusText) {
            $scope.setMainMenuEnabled(true);
            $location.path('/employees/load/false');
          }).
          error(function (data, status, headers, config, statusText) {
            $scope.setMainMenuEnabled(true);
          });
      };
    }
  ]).
  controller('ViewEmployeeController', ['$scope', '$location', 'northwindService',
    function ($scope, $location, northwindService) {
      $scope.readOnly = true;
      $scope.northwind = northwindService;
      $scope.employee = angular.copy($scope.northwind.selectedEmployee);
      $scope.headerText = 'View ' + $scope.northwind.getDisplayName($scope.northwind.selectedEmployee);
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
        $scope.setMainMenuEnabled(true);
        $location.path('/employees/load/false');
      };
      $scope.doOK = function () {
        $scope.setMainMenuEnabled(true);
        $location.path('/employees/load/false');
      };
    }
  ]);
