import { Exercise } from './Exercise';

export interface Workout {
    id: number;
    userId: number;
    userKnownAs: string;
    exercises: Exercise[];
}
