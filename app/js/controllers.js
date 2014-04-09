'use strict';

/* Controllers */

angular.module('myApp.controllers', [])
  .controller('MyCtrl1', [function() {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
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

    $http({
      url: '../NwndSvc.asmx/GetAllEmployees',
      method: "POST",
      data: "{}",
      headers: { 'Content-Type': 'application/json' }
    }).success(function (data, status, headers, config) {
      $scope.employees = data.d;
      return;
    }).error(function (data, status, headers, config) {
      $scope.data = data;
    });
  }]);
