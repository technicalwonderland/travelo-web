import {BrowserModule} from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CustomersComponent} from './customers/customers.component';
import {FormsModule} from '@angular/forms';
import {CustomerDetailsComponent} from './customer-details/customer-details.component';
import {MessagesComponent} from './messages/messages.component';
import {
    MatButtonModule, MatCardModule, MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule, MatInputModule,
    MatListModule, MatNativeDateModule, MatSelectModule,
    MatSliderModule,
    MatTabsModule,
    MatToolbarModule
} from '@angular/material';
import {HttpClientModule} from '@angular/common/http';
import {TripsComponent} from './trips/trips.component';
import {TripDetailsComponent} from './trip-details/trip-details.component';
import {CustomerSearchComponent} from './customer-search/customer-search.component';
import {CountryService} from './country.service';

@NgModule({
    declarations: [
        AppComponent,
        CustomersComponent,
        CustomerDetailsComponent,
        MessagesComponent,
        TripsComponent,
        TripDetailsComponent,
        CustomerSearchComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        FormsModule,
        MatSliderModule,
        HttpClientModule,
        MatButtonModule,
        MatTabsModule,
        MatListModule,
        MatIconModule,
        MatToolbarModule,
        MatFormFieldModule,
        MatInputModule,
        MatCardModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatSelectModule
    ],
    providers: [CountryService,
        {
            provide: APP_INITIALIZER,
            useFactory: countriesServiceFactory,
            deps: [CountryService],
            multi: true
        }],
    bootstrap: [AppComponent]
})
export class AppModule {
}

export function countriesServiceFactory(service: CountryService) {
    return () => service.load();
}
