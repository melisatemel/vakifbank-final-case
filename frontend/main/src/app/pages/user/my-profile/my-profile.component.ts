import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AddressService } from 'src/app/services/address.service';
import { CardService } from 'src/app/services/card.service';
import { MessageService } from 'src/app/services/message.service';
import { StorageService } from 'src/app/services/storage.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class MyProfileComponent implements OnInit {
  user: any = {};
  addAdress: boolean = false;
  addCard: boolean = false;
  addressForm: FormGroup;
  cardForm: FormGroup
  userId: number;
  messages: any = {};
  newMessage: string;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private storage: StorageService,     //kayıtlı olan id yi getirir storege
    private addressService: AddressService,
    private cardService: CardService,
    private toastr: ToastrService,
    private messageService: MessageService,
  ) {

  }

  ngOnInit(): void {
    this.userId = this.storage.getUser().userId;
    this.getUserInfos();
    this.initForm();
    this.getMessages();
  }

  getMessages(){
    this.messageService.getMessageForDealer(this.userId).subscribe((data: any)=>{
      this.messages = data.response;
    })
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

  getUserInfos() {
    this.userService.getUserById(this.userId).subscribe((data: any) => {
      this.user = data.response;
    });
  }
  selectedItem: number = 1;

  selectItem(itemNumber: number): void {
    this.selectedItem = itemNumber;
  }

  deleteAddress(addressId: number) {
    this.addressService.deleteAddressById(addressId).subscribe((data: any) => {
      this.getUserInfos();
    });
  }

  deleteCard(cardId: number) {
    this.cardService.deleteCardById(cardId).subscribe((data: any) => {
      this.getUserInfos();
    });
  }

  addNewAddress() {
    if (this.addressForm.invalid) {
      this.toastr.error("Please fill in all fields completely")
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

      this.addressService.createAddress(request).subscribe(()=>{
        this.getUserInfos();
        this.addAdress = false;
      })
    }
  }

  addCardAddress(){
    if (this.cardForm.invalid) {
      this.toastr.error("Please fill in all fields completely")
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

      this.cardService.createCard(request).subscribe(()=>{
        this.getUserInfos();
        this.addCard = false;
      })
    }
  }

  sendMessage(){
    const request = {
      "chatId": this.userId,
      "content": this.newMessage,
      "isAdmin": false,
    }
    this.messageService.createMessage(request).subscribe((data:any)=>{
      this.getMessages();
      this.newMessage = ''
    })
  }

}
