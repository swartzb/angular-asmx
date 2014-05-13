'use strict';

/* Controllers */

angular.module('myApp.controllers', ['ngRoute', 'myApp.services'])
  .controller('MyCtrl1', [function () {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$routeParams', '$window', '$location', 'northwindService',
    function ($scope, $routeParams, $window, $location, northwindService) {
      $scope.northwind = northwindService;

      $scope.addNewEmployee = function () {
        $scope.setMainMenuEnabled(false);
        $location.path('/employees/add');
        return;
      };

      $scope.editSelectedEmployee = function () {
        if ($scope.northwind.selectedEmployee) {
          $window.alert('Edit ' + $scope.northwind.selectedEmployee.LastName + ', ' + $scope.northwind.selectedEmployee.FirstName);
        }
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
        if ($scope.northwind.selectedEmployee) {
          $window.alert('View Details for ' + $scope.northwind.selectedEmployee.LastName + ', ' + $scope.northwind.selectedEmployee.FirstName);
        }
        return;
      };

      $scope.getDisplayName = function (employee) {
        var displayName = employee.LastName + ', ' + employee.FirstName;
        if (employee.TitleOfCourtesy) {
          displayName += ', ' + employee.TitleOfCourtesy;
        }
        if (employee.Title) {
          displayName += ', ' + employee.Title;
        }
        return displayName;
      };

      $scope.toggleSelect = function (employee) {
        $scope.northwind.selectedEmployee = (employee == $scope.northwind.selectedEmployee) ? null : employee;
        return;
      };

      if ($routeParams.loadVal == 'true') {
        northwindService.getAllEmployees();
      }
    }])
  .controller('AddEmployeeController', ['$scope', '$routeParams', '$location', 'northwindService',
    function ($scope, $routeParams, $location, northwindService) {
      $scope.northwind = northwindService;
      $scope.employee = {};
      $scope.headerText = 'Add New Employee';
      $scope.okButtonText = 'Add';
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
    }])
  .controller('IndexController', ['$scope', '$location', function ($scope, $location) {
    $scope.isMainMenuEnabled = true;
    $scope.setMainMenuEnabled = function (enabled) {
      $scope.isMainMenuEnabled = enabled;
    };
    $scope.employeesClickHandler = function () {
      $location.path('/employees/load/true');
    };

    $scope.getCssClasses = function (ngModelContoller) {
      return {
        'has-error': ngModelContoller.$invalid && ngModelContoller.$dirty,
        'has-success': ngModelContoller.$valid && ngModelContoller.$dirty
      };
    };
  }]);
