import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';


@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {
    form: FormGroup;

    private formSubmitAttempt: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
    ) {}

    ngOnInit() {
        this.form = this.formBuilder.group({
                username: ['', Validators.required],
                password: ['', Validators.required],
            }
        );
    }

    isFieldInvalid(field: string) {
        return (
            (!this.form.get(field).valid && this.form.get(field).touched) ||
            (this.form.get(field).untouched && this.formSubmitAttempt)
        );
    }

    onSubmit() {
        if (this.form.valid) {
            this.authService.signup(this.form.value);
        }
        this.formSubmitAttempt = true;
    }


}
