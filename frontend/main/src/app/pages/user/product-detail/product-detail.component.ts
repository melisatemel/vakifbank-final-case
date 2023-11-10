import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { ProductService } from "src/app/services/product.service";
import { ShoppingCartService } from "src/app/services/shopping-cart.service";
import { StorageService } from "src/app/services/storage.service";

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class ProductDetailComponent implements OnInit{
  productId: number;
  product: any = {};
  quantity: number = 1;

  constructor(private activatedRoute: ActivatedRoute, private productService: ProductService, private shoppingCartService: ShoppingCartService, private toastr: ToastrService, private storage: StorageService) {}

  ngOnInit() {
    this.productId = this.activatedRoute.snapshot.params['id'];
    this.getProductInfos();
  }

  private getProductInfos(){
    const userId = this.storage.getUser().userId;
    this.productService.getProductById(this.productId, userId).subscribe((data: any)=>{
      this.product = data.response;
    })
  }

  addToCart() {
    this.shoppingCartService.addProductToCart(this.productId).subscribe((data)=>{
      if(data.success){
        this.toastr.success('Successfully added to your cart!');
      }else{
        this.toastr.error(data.message);
      }
    })
  }
}
