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
          $window.alert('Delete ' + $scope.northwind.selectedEmployee.LastName + ', ' + $scope.northwind.selectedEmployee.FirstName);
        }
        return;
      };

      $scope.viewSelectedEmployeeDetails = function () {
        if ($scope.northwind.selectedEmployee) {
          $window.alert('View Details for ' + $scope.northwind.selectedEmployee.LastName + ', ' + $scope.northwind.selectedEmployee.FirstName);
        }
        return;
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
      $scope.headerText = 'Add New Employee';
      $scope.doCancel = function () {
        $scope.setMainMenuEnabled(true);
        $location.path('/employees/load/false');
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
  }]);
