import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-confirm-delete-dialog',
  template: `
    <h1 mat-dialog-title class="text-center text-danger mb-4">Delete Product</h1>
<div mat-dialog-content class="text-center mb-4">
  <p class="h5">Are you sure you want to delete the product '{{ data.productName }}'?</p>
</div>
<div mat-dialog-actions class="d-flex justify-content-center">
  <button mat-button color="primary" class="mr-3" (click)="onNoClick()">No</button>
  <button mat-button color="warn" [mat-dialog-close]="true" cdkFocusInitial>Yes</button>
</div>
  `,
})
export class PopupComponent {
  constructor(
    public dialogRef: MatDialogRef<PopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
