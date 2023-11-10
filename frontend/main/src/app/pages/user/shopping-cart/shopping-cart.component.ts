import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AddressService } from 'src/app/services/address.service';
import { CardService } from 'src/app/services/card.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { StorageService } from 'src/app/services/storage.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class ShoppingCartComponent implements OnInit {
  userId: any;
  user: any;
  products: any = [];
  shoppingCartId: number;
  disableMinus = false;
  showCompleteOrder = false;
  totalPrice: number = 0;
  selectedAddress: any = {};
  selectedCard: any = {};
  addAdress: boolean = false;
  addCard: boolean = false;
  addressForm: FormGroup;
  cardForm: FormGroup;
  messages: any = {};
  newMessage: string;
  successScreen = false;
  selectedPaymentOption: string = 'card';

  constructor(
    private storage: StorageService,
    private shoppingCartService: ShoppingCartService,
    private toastr: ToastrService,
    private userService: UserService,
    private fb: FormBuilder,
    private addressService: AddressService,
    private cardService: CardService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.userId = this.storage.getUser().userId;
    this.getUser();
    this.shoppingCartService
      .getUserShoppingCart(this.userId)
      .subscribe((data: any) => {
        this.shoppingCartId = data.response.id;
        this.products = data.response ? data.response.productQuantities : [];
        this.products.forEach((product: any) => {
          product.disableMinusButton = product.quantity === 1 ? true : false;
        });

        this.products.map((data: any) => {
          this.totalPrice = this.totalPrice + data.price * data.quantity;
        });
      });

    this.initForm();
  }

  clickQuantity(product: any, isMinus = false) {
    console.log(product);
    if (product.quantity == 1 && isMinus) {
      this.disableMinus = true;
    } else {
      this.shoppingCartService
        .addProductToCart(product.productId, isMinus)
        .subscribe((data) => {
          this.products = data.response.productQuantities;
        });
    }
  }

  deleteProductFromShoppingCart(product: any) {
    this.shoppingCartService
      .addProductToCart(product.productId, false, true)
      .subscribe((data) => {
        this.products = data.response.productQuantities;
      });
  }

  onSelectAddress(address: any) {
    this.selectedAddress = address;
  }

  onSelectCard(card: any) {
    this.selectedCard = card;
  }

  proceedToPayment() {
    this.showCompleteOrder = true;
    this.shoppingCartService.updateNumberOfItems();
    this.getUser();
  }

  getUser() {
    this.userService.getUserById(this.userId).subscribe((data: any) => {
      this.user = data.response;
    });
  }
  initForm() {
    this.addressForm = this.fb.group({
      addressLine1: ['', Validators.required],
      addressLine2: [''],
      city: ['', Validators.required],
      county: ['', Validators.required],
      postalCode: ['', Validators.required],
    });

    this.cardForm = this.fb.group({
      cardHolder: ['', Validators.required],
      cardNumber: [''],
      cvv: ['', Validators.required],
      expenseLimit: ['', Validators.required],
      expiryDate: ['', Validators.required],
    });
  }

  deleteAddress(addressId: number) {
    this.addressService.deleteAddressById(addressId).subscribe((data: any) => {
      this.getUser();
    });
  }

  addNewAddress() {
    if (this.addressForm.invalid) {
      this.toastr.error('Please fill in all fields completely');
    } else {
      const formData = this.addressForm.value;

      const request = {
        userId: this.userId,
        addressLine1: formData.addressLine1,
        addressLine2: formData.addressLine2,
        city: formData.city,
        county: formData.county,
        postalCode: formData.postalCode,
      };

      this.addressService.createAddress(request).subscribe(() => {
        this.getUser();
        this.addAdress = false;
      });
    }
  }

  deleteCard(cardId: number) {
    this.cardService.deleteCardById(cardId).subscribe((data: any) => {
      this.getUser();
    });
  }

  addCardAddress() {
    if (this.cardForm.invalid) {
      this.toastr.error('Please fill in all fields completely');
    } else {
      const formData = this.cardForm.value;

      const request = {
        userId: this.userId,
        cardHolder: formData.cardHolder,
        cardNumber: formData.cardNumber,
        cvv: formData.cvv,
        expenseLimit: formData.expenseLimit,
        expiryDate: formData.expiryDate,
      };

      this.cardService.createCard(request).subscribe(() => {
        this.getUser();
        this.addCard = false;
      });
    }
  }
  completeOrder() {
    const addressProperties = Object.keys(this.selectedAddress);
    const cardProperties = Object.keys(this.selectedCard);

    if (addressProperties.length > 0) {
      if (this.selectedPaymentOption === 'card') {
        if (cardProperties.length > 0) {
          this.updateShoppingCart(false, false);
        } else {
          this.showErrorToast('You have to fill out the address and payment fields.');
        }
      } else if (this.selectedPaymentOption === 'eft') {
        this.updateShoppingCart(true, false);
      } else if (this.selectedPaymentOption === 'account') {
        this.updateShoppingCart(false, true);
      }
    } else {
      this.showErrorToast('You have to fill out the address and payment fields.');
    }
    this.shoppingCartService.updateNumberOfItems();
  }

  private updateShoppingCart(isEft: boolean, isOpenAccount: boolean) {
    this.shoppingCartService
      .updateShoppingCart(
        this.shoppingCartId,
        isEft || isOpenAccount ? 0 : this.selectedCard.id,
        this.selectedAddress.id,
        isEft,
        isOpenAccount
      )
      .subscribe(
        (data: any) => {
          if (data.success) {
            this.successScreen = true;
          } else {
            this.showErrorToast(data.message);
          }
        },
        (error: any) => {
          this.showErrorToast('An error occurred while updating the shopping cart.');
        }
      );
  }

  private showErrorToast(message: string) {
    this.toastr.error(message);
  }


  cancel() {
    this.showCompleteOrder = false;
  }

  goToHomePage() {
    return this.router.navigate(['/dashboard']);
  }
}
