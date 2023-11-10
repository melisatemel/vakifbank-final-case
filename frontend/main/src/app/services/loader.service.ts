// loader.service.ts
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private loading = false;

  constructor(private spinner: NgxSpinnerService){}

  showLoader() {
    this.loading = true;
    this.spinner.show();
  }

  hideLoader() {
    this.loading = false;
    this.spinner.hide();
  }

  isLoading(): boolean {
    return this.loading;
  }
}
