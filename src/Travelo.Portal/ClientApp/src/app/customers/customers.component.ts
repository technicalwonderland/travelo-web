import {Component, OnInit} from '@angular/core';
import {Customer} from '../models/customer';
import {CustomerService} from '../services/customer.service';

@Component({
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
    customers: Customer[];

    constructor(private customerService: CustomerService) {
    }

    ngOnInit() {
        this.getCustomers();
    }

    getCustomers(): void {
        this.customerService.getCustomers().subscribe(customers => this.customers = customers);
    }

    addCustomer(firstName: string, lastName: string): void {
        if (firstName.trim().length === 0 || lastName.trim().length === 0) {
            return;
        }
        const customer = new Customer();
        customer.firstName = firstName;
        customer.lastName = lastName;
        customer.fullName = `${firstName} ${lastName}`;
        this.customerService.createCustomer(customer).subscribe(obj => {
            this.customers.push(obj);
        });
    }
}
