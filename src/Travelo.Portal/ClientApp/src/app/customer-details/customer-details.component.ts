import {Component, OnInit} from '@angular/core';
import {Customer} from '../models/customer';
import {ActivatedRoute} from '@angular/router';
import {CustomerService} from '../services/customer.service';
import {Location} from '@angular/common';

@Component({
    selector: 'app-customer-details',
    templateUrl: './customer-details.component.html',
    styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {

    customer: Customer;

    constructor(private route: ActivatedRoute,
                private customerService: CustomerService,
                private location: Location) {
    }

    ngOnInit(): void {
        this.getCustomer();
    }

    getCustomer(): void {
        const id = this.route.snapshot.paramMap.get('id');
        this.customerService.getCustomer(id)
            .subscribe(customer => this.customer = customer);
    }

    updateCustomer(): void {
        this.customerService.updateCustomer(this.customer)
            .subscribe(() => this.goBack());
    }

    goBack(): void {
        this.location.back();
    }

}
