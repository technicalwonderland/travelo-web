import {Component, OnInit} from '@angular/core';
import {Location} from '@angular/common';
import {Trip, TripStatus} from '../models/trip';
import {ActivatedRoute} from '@angular/router';
import {TripService} from '../services/trip.service';
import {CustomerService} from '../services/customer.service';
import {catchError, debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {Observable, Subject} from 'rxjs';
import {Customer} from '../models/customer';
import {Country} from '../models/country';
import {CountryService} from '../country.service';
import {MessageService} from '../services/message.service';
import {ErrorResponse} from '../models/error-response';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
    selector: 'app-trip-details',
    templateUrl: './trip-details.component.html',
    styleUrls: ['./trip-details.component.css']
})
export class TripDetailsComponent implements OnInit {

    trip: Trip;
    statusOptions = Object.keys(TripStatus);
    countries: Country[];
    customers$: Observable<Customer[]>;
    private searchFullNames = new Subject<string>();

    constructor(private route: ActivatedRoute,
                private tripService: TripService,
                private customerService: CustomerService,
                private location: Location,
                private countryService: CountryService,
                private messageService: MessageService) {
    }

    ngOnInit() {
        this.getTrip();
        this.customers$ = this.searchFullNames.pipe(
            debounceTime(250),
            distinctUntilChanged(),
            switchMap((term: string) => this.customerService.getCustomerByFullName(term)),
        );
        this.countries = this.countryService.getCountries();
    }

    getTrip(): void {
        const id = this.route.snapshot.paramMap.get('id');
        this.tripService.getTrip(id).subscribe(trip => this.trip = trip);
    }

    updateTrip(): void {
        this.tripService.updateTrip(this.trip)
            .subscribe(() => this.goBack());
    }

    goBack() {
        this.location.back();
    }

    addCustomer(customer: Customer) {
        if (!this.trip.customers.some(e => e.id === customer.id)) {
            this.tripService.addCustomer(this.trip.id, customer.id)
                .subscribe((data) => this.trip.customers.push(customer),
                    (error: HttpErrorResponse) => {
                        this.messageService.add(`${(error.error as ErrorResponse).message}`);
                    });
        }
    }
}
