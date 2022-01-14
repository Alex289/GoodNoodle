describe('Logout page Tests', () => {
  before(() => {
    cy.visit('/register');

    cy.get('#input-name').type('Cypress Login');
    cy.get('#input-email').type('cypress@login.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('#input-re-password').type('Cypress1#');
    cy.get('.custom-checkbox').click();
    cy.get('button[status="primary"]').contains('Register').click();
  });

  it('Should visit the login page', () => {
    cy.visit('/login');
    cy.contains('Login');
  });

  it('Should login user', () => {
    cy.get('#input-email').type('cypress@login.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('button[status="primary"]').contains('Log In').click();
  });

  it('Should have logged in user', () => {
    cy.get('nb-user').should('exist');
    cy.get('.user-name').contains('Cypress Login').should('be.visible');
  });

  it('Should logout user', () => {
    cy.get('#logout-btn').click();
    cy.location('pathname').should('eq', '/');
  });
});
