import { Component, Input, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { RedeSocial } from '@app/models/RedeSocial';
import { RedeSocialService } from '@app/services/rede-social.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redes-sociais',
  templateUrl: './redes-sociais.component.html',
  styleUrls: ['./redes-sociais.component.scss'],
})
export class RedesSociaisComponent {
  modalRef: BsModalRef;
  @Input() eventoId = 0;
  public formRS: FormGroup;
  public redeSocialAtual = { id: 0, nome: '', indice: 0 };

  public get redesSociais(): FormArray {
    return this.formRS.get('redesSociais') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private redeSocialService: RedeSocialService,
    private modalService: BsModalService,
    private router: Router
  ) {}

  ngOnInit() {
    this.carregarRedesSociais(this.eventoId);
    this.validation();
  }

  private carregarRedesSociais(id: number = 0): void {
    let isEvento = this.eventoId !== 0 ? true : false;

    this.spinner.show();
    this.redeSocialService
      .getRedesSociais(id, isEvento)
      .subscribe({
        next: (redeSocialRetorno: RedeSocial[]) => {
          redeSocialRetorno.forEach((redeSocial) => {
            this.redesSociais.push(this.criarRedeSocial(redeSocial));
          });
        },
        error: (error: any) => {
          this.toastr.error('Erro ao tentar carregar Rede Social', 'Erro');
          console.error(error);
        },
      })
      .add(() => this.spinner.hide());
  }

  public validation(): void {
    this.formRS = this.fb.group({
      redesSociais: this.fb.array([]),
    });
  }

  adicionarRedeSocial(): void {
    this.redesSociais.push(this.criarRedeSocial({ id: 0 } as RedeSocial));
  }

  criarRedeSocial(redeSocial: RedeSocial): FormGroup {
    return this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required],
    });
  }

  public retornaTitulo(nome: string): string {
    return nome === null || nome === '' ? 'Rede Social' : `${nome}`;
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public salvarRedesSociais(): void {
    let isEvento = false;
    if (this.eventoId !== 0) {
      isEvento = true;
    } else {
      isEvento = false;
    }

    if (this.formRS.valid) {
      this.spinner.show();
      this.redeSocialService
        .saveRedesSociais(
          this.eventoId,
          isEvento,
          this.formRS.value.redesSociais
        )
        .subscribe({
          next: () => {
            this.toastr.success(
              'Redes Sociais foram salvas com Sucesso!',
              'Sucesso!'
            );
            this.formRS.reset();

            // Then remove the old form controls
            while (this.redesSociais.length !== 0) {
              this.redesSociais.removeAt(0);
            }

            // Then load redes sociais again
            this.carregarRedesSociais(this.eventoId);
          },
          error: (error: any) => {
            this.toastr.error(
              `Erro ao tentar salvar Redes Sociais. ${error.error}`,
              'Erro'
            );
            console.error(error);
          },
        })
        .add(() => this.spinner.hide());
    }
  }

  public removerRedeSocial(template: TemplateRef<any>, indice: number): void {
    this.redeSocialAtual.id = this.redesSociais.get(indice + '.id').value;
    this.redeSocialAtual.nome = this.redesSociais.get(indice + '.nome').value;
    this.redeSocialAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmDeleteRedeSocial(): void {
    let origem = 'palestrante';
    this.modalRef.hide();
    this.spinner.show();

    if (this.eventoId !== 0) origem = 'evento';

    this.redeSocialService
      .deleteRedeSocial(origem, this.eventoId, this.redeSocialAtual.id)
      .subscribe(
        () => {
          this.toastr.success('Rede Social deletado com sucesso', 'Sucesso');
          this.redesSociais.removeAt(this.redeSocialAtual.indice);
        },
        (error: any) => {
          this.toastr.error(
            `Erro ao tentar deletar o Rede Social ${this.redeSocialAtual.id}`,
            'Erro'
          );
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());
  }

  declineDeleteRedeSocial(): void {
    this.modalRef.hide();
  }
}
