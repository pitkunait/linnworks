import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { UserLogin } from '../interfaces/userLogin.interface';
import { TokenService } from './token.service';
import { TokenPair } from '../interfaces/tokenPair.interface';
import { UserSignup } from '../interfaces/userSignup.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';


@Injectable({
    providedIn: 'root',
})
export class AuthService {
    public loggedIn = new BehaviorSubject<boolean>(false);

    constructor(
        private router: Router,
        private tokenService: TokenService,
        private http: HttpClient,
    ) {
        if (this.tokenService.hasToken()) {
            this.loggedIn.next(true);
        }
    }

    login(user: UserLogin) {
        this.http
            .post(`${environment.baseUrl}/auth/login`, user)
            .subscribe(async (res: TokenPair) => {
                this.tokenService.setTokenPair(res);
                this.loggedIn.next(true);
                await this.router.navigate(['/dashboard']);
            });
    }

    signup(user: UserSignup) {
        this.http
            .post(`${environment.baseUrl}/auth/signup`, {
                username: user.username,
                password: user.password,
                first_name: user.firstName,
                last_name: user.lastName,
                email: user.email,
            })
            .subscribe(async (res: any) => {
                await this.router.navigate(['auth/login']);
            });
    }

    logout() {
        this.loggedIn.next(false);
        this.tokenService.deleteTokens();
        this.router.navigate(['/auth/login']);
    }
}
