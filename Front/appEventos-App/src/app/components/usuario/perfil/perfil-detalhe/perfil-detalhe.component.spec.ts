import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerfilDetalheComponent } from './perfil-detalhe.component';

describe('PerfilDetalheComponent', () => {
  let component: PerfilDetalheComponent;
  let fixture: ComponentFixture<PerfilDetalheComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PerfilDetalheComponent]
    });
    fixture = TestBed.createComponent(PerfilDetalheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
