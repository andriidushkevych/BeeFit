import { Component, OnInit } from '@angular/core';
import { Workout } from '../models/Workout';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.css']
})
export class WorkoutComponent implements OnInit {
  workout: Workout;
  userId: number;
  user: User;
  constructor(private authService: AuthService,
              private userService: UserService) { }

  ngOnInit() {
    this.userId = this.authService.decodedToken.nameid;
    this.userService.getWorkout(this.userId).subscribe(res => {
      this.workout = res;
    });
  }

}
