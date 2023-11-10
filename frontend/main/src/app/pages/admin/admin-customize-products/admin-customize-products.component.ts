import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { PopupComponent } from '../../ui-components/popup/popup.component';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-customize-products',
  templateUrl: './admin-customize-products.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppAdminCustomizeProductsComponent implements OnInit{
  showEdit = false;
  constructor(private router: Router, private productService: ProductService, private dialog: MatDialog, private storage: StorageService){}
  selectedProduct: any = {};
  products: any;
  ngOnInit(): void {
    this.getAllProducts()
  }

  getAllProducts(){
    const userId = this.storage.getUser().userId;
    this.productService.getAllProduct(userId).subscribe((data:any)=>{
      this.products = data.response;
    })
  }

  backToDashboard(){
    this.router.navigate(['admin/dashboard'])
  }

  editProduct(product: any) {
    this.showEdit = true;
    this.selectedProduct = { ...product };
  }

  submitEditForm() {
    const editedProductRequest = {
      productId: this.selectedProduct.productId,
      name: this.selectedProduct.name,
      description: this.selectedProduct.description,
      price: this.selectedProduct.price,
      stockQuantity: this.selectedProduct.stockQuantity,
      image: this.selectedProduct.image,
    };
    this.productService.updateProduct(editedProductRequest).subscribe(()=>{
      this.selectedProduct = {};
      this.showEdit = false;
      this.getAllProducts()
    })
  }


  confirmDelete(product: any): void {
    const dialogRef = this.dialog.open(PopupComponent, {
      width: '400px',
      data: { productName: product.name }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.deleteProduct(product.productId).subscribe(()=>{
          this.getAllProducts()
        })
      }
    });
  }
}
