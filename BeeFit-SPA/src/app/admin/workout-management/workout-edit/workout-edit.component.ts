import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from 'src/app/models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
import { Workout } from 'src/app/models/Workout';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-workout-edit',
  templateUrl: './workout-edit.component.html',
  styleUrls: ['./workout-edit.component.css']
})
export class WorkoutEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  user: User;
  userId: number;
  workout: Workout;
  photoUrl: string;
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute,
              private alertify: AlertifyService,
              private adminService: AdminService,
              private userService: UserService,
              private authService: AuthService) { }

  ngOnInit() {
    this.userId = this.route.snapshot.params['id'];
    this.userService.getUser(this.userId).subscribe(res => {
      this.user = res;
      this.photoUrl = this.user.photoUrl;
    }, error => {
      this.alertify.error(error);
    });
    this.userService.getWorkout(this.userId).subscribe(res => {
      this.workout = res;
    }, error => {
      this.alertify.error(error);
    });
  }

  updateWorkout() {
    this.adminService.editWorkout(this.authService.decodedToken.nameid, this.workout.exercises)
        .subscribe(next => {
          this.alertify.success('Updated successfully!');
          this.editForm.reset(this.workout);
        }, error => {
          this.alertify.error(error);
        });
  }
}
