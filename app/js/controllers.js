'use strict';

/* Controllers */

angular.module('myApp.controllers', ['myApp.services'])
  .controller('MyCtrl1', [function () {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$window', 'northwindService', function ($scope, $window, northwindService) {
    $scope.selectedEmployee = null;

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

    $scope.toggleSelect = function (employee) {
      $scope.selectedEmployee = (employee == $scope.selectedEmployee) ? null : employee;
      return;
    };

    northwindService.getAllEmployees().success(function (data, status, headers, config) {
      $scope.employees = data.d;
      return;
    }).error(function (data, status, headers, config) {
      $scope.data = data;
    });
  }]);
