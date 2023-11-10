import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import {  Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/services/product.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { StorageService } from 'src/app/services/storage.service';

interface productcards {
  id: number;
  imgSrc: string;
  title: string;
  price: string;
}


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppDashboardComponent implements OnInit {
  filteredProductCards: productcards[] = [];
  productNameFilter = '';
  minPriceFilter: number | null;
  maxPriceFilter: number| null;
  productCards: productcards[] = [];

  constructor(private productService: ProductService, private router: Router, private toastr: ToastrService, private storage: StorageService, private shoppingCartService: ShoppingCartService) {}

  ngOnInit(): void {
    this.shoppingCartService.updateNumberOfItems();
    this.getProducts();
  }

  getProducts(){
    const userId = this.storage.getUser().userId;
    this.productService.getAllProduct(userId).subscribe((data) =>
      data.response
        .filter((data: any) => data.stockQuantity !== 0)
        .map((item: any) => {
          this.productCards.push({
            id: item.productId,
            imgSrc: item.image,
            title: item.name,
            price: item.price,
          });
        })
    );

    this.filteredProductCards = this.productCards;
  }

  navigateToProductDetail(id: number) {
    this.router.navigate(['/product-detail', id]);
  }

  applyFilters() {
    this.filteredProductCards = this.productCards.filter((product) => {
      const productNameMatch = product.title.toLowerCase().includes(this.productNameFilter.toLowerCase());
      const minPriceMatch = this.minPriceFilter ? parseFloat(product.price) >= this.minPriceFilter : true;
      const maxPriceMatch = this.maxPriceFilter ? parseFloat(product.price) <= this.maxPriceFilter : true;

      return productNameMatch && minPriceMatch && maxPriceMatch;
    });
  }

  clearFilters(){
    this.filteredProductCards = this.productCards;
    this.productNameFilter = '';
    this.minPriceFilter = null;
    this.maxPriceFilter= null;
  }
}
