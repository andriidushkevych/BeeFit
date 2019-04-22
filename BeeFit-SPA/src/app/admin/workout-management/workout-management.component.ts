import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AdminService } from 'src/app/services/admin.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AlertifyService } from 'src/app/services/alertify.service';
import { UserService } from 'src/app/services/user.service';
import { Workout } from 'src/app/models/Workout';

@Component({
  selector: 'app-workout-management',
  templateUrl: './workout-management.component.html',
  styleUrls: ['./workout-management.component.css']
})
export class WorkoutManagementComponent implements OnInit {
  users: User[];

  constructor(private adminService: AdminService,
              private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      this.alertify.error(error);
    });
  }
}
