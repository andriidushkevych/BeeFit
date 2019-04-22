import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Workout } from '../models/Workout';
import { UserService } from '../services/user.service';

@Injectable()
export class WorkoutEditResolver implements Resolve<Workout> {
  constructor(private userService: UserService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Workout> {
    return this.userService.getWorkout(route.params['id']);
  }
}
