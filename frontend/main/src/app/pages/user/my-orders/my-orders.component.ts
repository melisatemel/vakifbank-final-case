import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ReportService } from 'src/app/services/report.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class MyOrdersComponent implements OnInit {
  orders: any = [];
  filteredOrders: any = [];
  filterDate: string = '';
  filterMinPrice: number | null;
  filterMaxPrice: number | null;
  filterCanceled: boolean = false;
  filterNotCompleted: boolean = false;
  filterCompleted: boolean = false;
  showGetReport = false;
  report: any;
  userId: number;
  selectedReportType: string = 'daily';
  filteredReport: any[] = [];

  constructor(
    private storage: StorageService,
    private shoppingCartService: ShoppingCartService,
    private toastr: ToastrService,
    private reportService: ReportService
  ) {}

  ngOnInit() {
    this.getOrders();
  }

  getOrders() {
    this.userId = this.storage.getUser().userId;
    this.shoppingCartService
      .getComplatedShoppingCart(this.userId)
      .subscribe((data: any) => {
        this.orders = data.response ?? [];
        this.applyFilters();
      });

    this.getReport();
  }

  calculateTotalPrice(order: any): number {
    let totalPrice = 0;

    order.productQuantities.forEach((product: any) => {
      totalPrice += product.price * product.quantity;
    });

    return totalPrice;
  }

  cancelProduct(order: any) {
    this.shoppingCartService.cancelShoppingCart(order.id).subscribe(() => {
      this.getOrders();
    });
  }

  applyFilters() {
    this.filteredOrders = this.orders.filter((order: any) => {
      // Tarih filtresi
      const dateMatch =
        !this.filterDate || order.createdAt.includes(this.filterDate);

      // Fiyat filtresi
      const minPriceMatch =
        !this.filterMinPrice ||
        this.calculateTotalPrice(order) >= this.filterMinPrice;
      const maxPriceMatch =
        !this.filterMaxPrice ||
        this.calculateTotalPrice(order) <= this.filterMaxPrice;

      // Durum filtreleri
      const canceledMatch = !this.filterCanceled || order.isCanceled;
      const notCompletedMatch =
        !this.filterNotCompleted || (order.isActive && !order.isCanceled);
      const completedMatch =
        !this.filterCompleted || (!order.isActive && !order.isCanceled);

      return (
        dateMatch &&
        minPriceMatch &&
        maxPriceMatch &&
        canceledMatch &&
        notCompletedMatch &&
        completedMatch
      );
    });
  }

  clearFilters() {
    this.filterDate = '';
    this.filterMinPrice = null;
    this.filterMaxPrice = null;
    this.filterCanceled = false;
    this.filterNotCompleted = false;
    this.filterCompleted = false;
    this.applyFilters();
  }

  getReport() {
    this.reportService.getOrderReports(this.userId).subscribe((data: any) => {
      this.report = data.response;
      this.updateReport();
    });
  }

  updateReport() {
    this.filteredReport = this.filterReportByType(
      this.report,
      this.selectedReportType
    );
  }

  filterReportByType(report: any[], reportType: string): any[] {
    const currentDate = new Date();

    switch (reportType) {
      case 'daily':
        // Günlük rapor
        return report.filter((item) =>
          this.isSameDay(new Date(item.orderDate), currentDate)
        );
      case 'weekly':
        // Haftalık rapor
        const startOfWeek = new Date(currentDate);
        startOfWeek.setDate(currentDate.getDate() - currentDate.getDay());
        return report.filter((item) => new Date(item.orderDate) >= startOfWeek);
      case 'yearly':
        // Yıllık rapor
        const startOfYear = new Date(currentDate.getFullYear(), 0, 1);
        return report.filter((item) => new Date(item.orderDate) >= startOfYear);
      default:
        return report;
    }
  }

  isSameDay(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }

  calculateFilteredTotalPrice(): number {
    let totalPrice = 0;

    this.filteredReport.forEach((reportItem: any) => {
      totalPrice += reportItem.productPrice * reportItem.productQuantity;
    });

    return totalPrice;
  }
}
