import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PalestranteDetalheComponent } from './palestrante-detalhe.component';

describe('PalestranteDetalheComponent', () => {
  let component: PalestranteDetalheComponent;
  let fixture: ComponentFixture<PalestranteDetalheComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PalestranteDetalheComponent]
    });
    fixture = TestBed.createComponent(PalestranteDetalheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
