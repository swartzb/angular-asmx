'use strict';

/* Controllers */

angular.module('myApp.controllers', ['myApp.services'])
  .controller('MyCtrl1', [function () {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$window', 'northwindService', function ($scope, $window, northwindService) {
    $scope.northwind = northwindService;

    $scope.addNewEmployee = function () {
      $window.alert('Add New Employee ');
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

    northwindService.getAllEmployees();
  }]);
