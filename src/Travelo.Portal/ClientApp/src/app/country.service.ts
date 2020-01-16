import {Injectable} from '@angular/core';
import {Country} from './models/country';
import {HttpClient} from '@angular/common/http';
import {BaseService} from './services/baseservice';
import {MessageService} from './services/message.service';

@Injectable()
export class CountryService extends BaseService {
    private countries: Country[] = [];
    private countryApiUrl = 'https://restcountries.eu/rest/v2/all?fields=name';

    constructor(protected httpClient: HttpClient,
                protected messageService: MessageService) {
        super(httpClient, messageService);
    }

    getCountries(): Country[] {
        return this.countries;
    }

    load(): Promise<any> {
        return new Promise((resolve, reject) => {
            this.httpClient
                .get<Country[]>(this.countryApiUrl).pipe()
                .subscribe(countries => {
                    this.countries = countries;
                    resolve(true);
                });
        });
    }
}
