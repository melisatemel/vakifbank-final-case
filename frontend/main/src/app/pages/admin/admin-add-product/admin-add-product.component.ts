import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-admin-add-product',
  templateUrl: './admin-add-product.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppAdminAddProductComponent {
  showSuccess = false;
  product = {
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
    image: '',
  };

  constructor(private productService: ProductService, private router: Router, private toastr: ToastrService) {}

  onSubmit() {
    if (this.isProductModified()) {
      this.productService.addProduct(this.product).subscribe((data: any) => {
        this.showSuccess = true;
      });
    }else{
      this.toastr.warning("Please fill in all fields.")
    }
  }

  isProductModified(): boolean {
    return (
      this.product.name !== '' &&
      this.product.description !== '' &&
      this.product.price !== 0 &&
      this.product.stockQuantity !== 0 &&
      this.product.image !== ''
    );
  }

  navigate(path: string){
    this.product = {
      name: '',
      description: '',
      price: 0,
      stockQuantity: 0,
      image: '',
    };
    this.showSuccess = false;
    this.router.navigate([path]);
  }

  backToDashboard(){
    this.router.navigate(['admin/dashboard'])
  }
}
