import {MessageService} from './message.service';
import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {ErrorResponse} from '../models/error-response';

export class BaseService {

    constructor(protected httpClient: HttpClient, protected messageService: MessageService) {
    }

    protected log(message: string): void {
        alert(message);
    }

    protected handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            // TODO: better job of transforming error for user
            const errorResponse = (error.error as ErrorResponse);
            if (!!errorResponse) {
                this.log(`${errorResponse.message}`);
            }


            return of(result as T);
        };
    }
}
