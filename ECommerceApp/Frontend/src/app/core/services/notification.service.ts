import { Injectable } from '@angular/core';
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private _snackBar: MatSnackBar) { }
  durationInSeconds = 5000000;

  // Function to show error message
  showError(message: string) {
    this._snackBar.open(message,"", {
      duration: this.durationInSeconds,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snack-error']
    });
  }
  // Function to show success message
  showSuccess(message: string) {
    this._snackBar.open(message,"", {
      duration: this.durationInSeconds,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snack-success']
    });
  }
  // Function to show info message
  showInfo(message: string) {
    this._snackBar.open(message,"", {
      duration: this.durationInSeconds,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snack-info']
    });
  }
  // Function to show warning message
  showWarning(message: string) {
    this._snackBar.open(message,"", {
      duration: this.durationInSeconds,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snack-warn']
    });
  }
}
