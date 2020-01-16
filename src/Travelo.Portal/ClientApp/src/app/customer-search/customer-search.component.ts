import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {CustomerService} from '../services/customer.service';
import {Customer} from '../models/customer';

@Component({
    selector: 'app-customer-search',
    templateUrl: './customer-search.component.html',
    styleUrls: ['./customer-search.component.css']
})

export class CustomerSearchComponent implements OnInit {
    customers$: Observable<Customer[]>;
    private searchFullNames = new Subject<string>();
    @Output() customerSelected: EventEmitter<Customer> = new EventEmitter<Customer>();

    constructor(private customerService: CustomerService) {
    }

    search(term: string): void {
        this.searchFullNames.next(term);
    }

    selectCustomer(customer: Customer): void {
        this.customerSelected.emit(customer);
        this.searchFullNames.next('');
    }

    ngOnInit(): void {
        this.customers$ = this.searchFullNames.pipe(
            debounceTime(250),
            distinctUntilChanged(),
            switchMap((term: string) => this.customerService.getCustomerByFullName(term))
        );
    }
}
