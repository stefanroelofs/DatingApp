import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  busy() {
    this.busyRequestCount++;
    console.log("busy method executed: " + this.busyRequestCount);
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(0,43,54,0.8)',
      color: '#FFFFFF'
    })


  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
    console.log("idle method executed: " + this.busyRequestCount);

  }
}
