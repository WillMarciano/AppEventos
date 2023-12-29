import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {
  isCollapsed = true;

  constructor(private router: Router, private accountService: AccountService) {}

  ngOnInit(): void {}

  logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl('/usuario/login');
  }

  showMenu(): boolean {
    return this.router.url != '/usuario/login';
  }
}
