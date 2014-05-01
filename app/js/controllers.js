'use strict';

/* Controllers */

angular.module('myApp.controllers', ['myApp.services'])
  .controller('MyCtrl1', [function () {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$window', 'northwindService', function ($scope, $window, northwindService) {
    $scope.northwind = northwindService;
    $scope.selectedEmployee = null;

    $scope.addNewEmployee = function () {
      $window.alert('Add New Employee ');
      return;
    };

    $scope.editSelectedEmployee = function () {
      if ($scope.selectedEmployee) {
        $window.alert('Edit ' + $scope.selectedEmployee.LastName + ', ' + $scope.selectedEmployee.FirstName);
      }
      return;
    };

    $scope.deleteSelectedEmployee = function () {
      if ($scope.selectedEmployee) {
        $window.alert('Delete ' + $scope.selectedEmployee.LastName + ', ' + $scope.selectedEmployee.FirstName);
      }
      return;
    };

    $scope.viewSelectedEmployeeDetails = function () {
      if ($scope.selectedEmployee) {
        $window.alert('View Details for ' + $scope.selectedEmployee.LastName + ', ' + $scope.selectedEmployee.FirstName);
      }
      return;
    };

    $scope.toggleSelect = function (employee) {
      $scope.selectedEmployee = (employee == $scope.selectedEmployee) ? null : employee;
      return;
    };

    northwindService.getAllEmployees();
  }]);
