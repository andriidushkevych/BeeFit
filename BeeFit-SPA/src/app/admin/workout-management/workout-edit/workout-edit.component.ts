import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from 'src/app/models/user';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { UserService } from 'src/app/services/user.service';
import { Workout } from 'src/app/models/Workout';
import { AdminService } from 'src/app/services/admin.service';
import { Exercise } from 'src/app/models/Exercise';

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
  exercises: Exercise[];
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute,
              private router: Router,
              private alertify: AlertifyService,
              private adminService: AdminService,
              private userService: UserService) { }

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
      this.exercises = this.workout.exercises;
    }, error => {
      this.alertify.error(error);
    });
  }

  updateWorkout() {
    this.adminService.editWorkout(this.user, this.exercises)
        .subscribe(next => {
          this.alertify.success('Updated successfully!');
          this.router.navigate(['members', this.userId]);
        }, error => {
          this.alertify.error(error);
        });
  }
}
