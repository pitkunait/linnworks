import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';


@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {

    constructor(public authService: AuthService, private router: Router) { }

    ngOnInit(): void {
    }

    login() {
        this.router.navigate(['/auth/login']);
    }

}
