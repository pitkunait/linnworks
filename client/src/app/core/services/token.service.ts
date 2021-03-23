import { Injectable } from '@angular/core';
import { Token } from '../enums/token.enum';
import { TokenPair } from '../interfaces/tokenPair.interface';


@Injectable({
    providedIn: 'root',
})
export class TokenService {

    constructor() { }


    hasToken() {
        return !!localStorage.getItem(Token.REFRESH);
    }

    getHeader() {
        const access = this.getAccessToken();
        return { Authorization: `Bearer ${access}` };
    }

    getRefreshToken() {
        return localStorage.getItem(Token.REFRESH);
    }

    getAccessToken() {
        return localStorage.getItem(Token.ACCESS);
    }

    setRefreshToken(token: string) {
        localStorage.setItem(Token.REFRESH, token);
    }

    setAccessToken(token: string) {
        localStorage.setItem(Token.ACCESS, token);
    }

    setTokenPair(tokenPair: TokenPair) {
        this.setRefreshToken(tokenPair.refreshToken);
        this.setAccessToken(tokenPair.accessToken);
    }

    deleteTokens() {
        localStorage.removeItem(Token.ACCESS);
        localStorage.removeItem(Token.REFRESH);
    }


}
