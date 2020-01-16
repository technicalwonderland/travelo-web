import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {Customer} from '../models/customer';
import {MessageService} from './message.service';
import {HttpClient} from '@angular/common/http';
import {HttpOptionsJsonContent} from './host';
import {catchError, map} from 'rxjs/operators';
import {environment} from '../../environments/environment';
import {BaseService} from './baseservice';

@Injectable({
    providedIn: 'root'
})
export class CustomerService extends BaseService {

    private customerApi = environment.baseUrl + '/api/customers';

    constructor(protected httpClient: HttpClient, protected messageService: MessageService) {
        super(httpClient, messageService);
    }

    getCustomers(): Observable<Customer[]> {
        return this.httpClient.get<Customer[]>(this.customerApi).pipe(
            catchError(this.handleError<Customer[]>('getCustomers', [])));
    }

    getCustomer(id: string): Observable<Customer> {
        return this.httpClient.get<Customer>(`${this.customerApi}/${id}`).pipe(
            catchError(this.handleError<Customer>('getCustomer')));
    }

    getCustomerByFullName(fullName: string): Observable<Customer[]> {
        return this.httpClient.get<Customer[]>(`${this.customerApi}/fullname/${fullName}`)
            .pipe(catchError(err => of({} as Customer[])));
    }

    updateCustomer(customer: Customer): Observable<object> {
        return this.httpClient.put(`${this.customerApi}/${customer.id}`, customer, HttpOptionsJsonContent).pipe(
            catchError(this.handleError<Customer>('updateCustomer')));
    }

    createCustomer(customer: Customer): Observable<Customer> {
        return this.httpClient.post(this.customerApi, customer, {headers: HttpOptionsJsonContent.headers, observe: 'response'}).pipe(
            map((resp) => {
                const splitLocation = resp.headers.get('location').split('/');
                customer.id = splitLocation[splitLocation.length - 1];
                return customer;
            }),
            catchError(this.handleError<Customer>('createCustomer'))
        );
    }
}
