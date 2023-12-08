import { EventoService } from '@app/services/evento.service';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  evento!: Evento;
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      dateInputFormat: 'DD-MM-YYYY hh:mm',
      isAnimated: true,
      adaptivePosition: true,
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(
    private fb: FormBuilder,
    private router: ActivatedRoute,
    private eventoService: EventoService
  ) {}

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if (eventoIdParam != null) {
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
        },
        error: (error: any) => {
          console.error(error);
        },
        complete: () => {},
      });
    }
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', [Validators.required]],
      dataEvento: ['', [Validators.required]],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', [Validators.required]],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoValidacao: FormControl): any {
    return { 'is-invalid': campoValidacao.errors && campoValidacao.touched };
  }
}
