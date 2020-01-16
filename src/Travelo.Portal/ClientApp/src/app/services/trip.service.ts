import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {environment} from '../../environments/environment';
import {MessageService} from './message.service';
import {Trip} from '../models/trip';
import {BaseService} from './baseservice';
import {HttpOptionsJsonContent} from './host';

@Injectable({
    providedIn: 'root'
})
export class TripService extends BaseService {

    private tripApi = environment.baseUrl + '/api/trips';

    constructor(protected httpClient: HttpClient, protected messageService: MessageService) {
        super(httpClient, messageService);
    }

    getTrips(): Observable<Trip[]> {
        return this.httpClient.get<Trip[]>(this.tripApi).pipe(
            catchError(this.handleError<Trip[]>('getTrips', [])));
    }

    getTrip(id: string): Observable<Trip> {
        return this.httpClient.get<Trip>(`${this.tripApi}/${id}`).pipe(
            catchError(this.handleError<Trip>('getTrip')));
    }

    updateTrip(trip: Trip): Observable<object> {
        return this.httpClient.put(`${this.tripApi}/${trip.id}`, trip, HttpOptionsJsonContent).pipe(
            catchError(this.handleError<Trip>('updateTrip')));
    }

    addTrip(trip: Trip): Observable<Trip> {
        return this.httpClient.post(this.tripApi, trip, {headers: HttpOptionsJsonContent.headers, observe: 'response'}).pipe(
            map((resp) => {
                const splitLocation = resp.headers.get('location').split('/');
                trip.id = splitLocation[splitLocation.length - 1];
                return trip;
            }),
            catchError(this.handleError<Trip>('createTrip'))
        );
    }

    addCustomer(tripId: string, customerId: string): Observable<object> {
        return this.httpClient.post(`${this.tripApi}/${tripId}/customers/${customerId}`, HttpOptionsJsonContent);
    }
}
