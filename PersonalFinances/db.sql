DROP DATABASE finance;

CREATE DATABASE finance;

CREATE TABLE finance.tb_user (
    id BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(60) NOT NULL,
    lastName VARCHAR(60) NOT NULL,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(128) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    photo LONGBLOB,
    PRIMARY KEY (id),
    UNIQUE(email)
);

CREATE TABLE finance.tb_category (
    id BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NOT NULL,
    type VARCHAR(1) DEFAULT 'D',
    PRIMARY KEY (id)
);

CREATE TABLE finance.tb_transaction (
    id BIGINT NOT NULL AUTO_INCREMENT,
    description VARCHAR(255) NOT NULL,
    value DOUBLE NOT NULL,
    type VARCHAR(1) DEFAULT 'D',
    date DATE DEFAULT CURDATE(),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    fk_user BIGINT NOT NULL,
    fk_category BIGINT NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_transactionsXuser
        FOREIGN KEY (fk_user) REFERENCES tb_user (id),
    CONSTRAINT fk_transactionXCategory
        FOREIGN KEY (fk_category) REFERENCES tb_category (id)
);

/* Add 3 Users tb_user */
INSERT INTO finance.tb_user (name, lastName, email,  password)
    VALUES ('Olivia Johnson', 'Lee', 'olivia.lee@gmail.com', 'f9G@x2#s');
INSERT INTO finance.tb_user (name, lastName, email,  password)
    VALUES ('Ethan Wilson', 'Wong', 'ethan.wong@hotmail.com', 'D3v$P7@t');
INSERT INTO finance.tb_user (name, lastName, email,  password)
    VALUES ('Ava Chen', 'Patel', 'ava.patel@yahoo.com', 'B1r#K9%p');

/* Add 11 Expenses tb_category */
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Housing', 'Rent/mortgage payments, utilities, repairs, and maintenance', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Transportation', 'Car payments, gas, maintenance, and public transportation', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Food', 'Groceries and dining out expenses', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Entertainment', 'Expenses for movies, concerts, and other leisure activities', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Health', 'Expenses for health insurance, doctor visits, and medication', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Clothing', 'Expenses for clothing, shoes, and accessories', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Education', 'Expenses for tuition, books, and other education-related costs', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Debt', 'Payments for loans, credit cards, and other debt', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Taxes', 'Income taxes, property taxes, and other taxes', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Travel', 'Expenses for flights, hotels, and other travel-related costs', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Others', 'Expenses that do not fit into any other category', 'D');

/* Add 7 Earnings tb_category */
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Personal Income', 'Salary, commissions, bonuses, awards, tips, and other forms of compensation that an individual receives from their work.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Business Income', 'Net profit, revenue, return on investment (ROI), and other financial metrics related to a company’s performance.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Investment Income', 'Interest, dividends, capital gains, and other forms of income generated from financial investments.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Passive Income', 'Rental income, royalties from intellectual property, online advertising income, and other forms of income generated without the need for active work.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Gambling/Lottery Winnings', 'Winnings from gambling and lotteries.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Other', 'Income that does not fit into any of the categories.', 'R');

/* Add 7 Expenses Transactions tb_transaction */
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Electric Bill', 5.0, '2023-04-10', 1, 3);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Groceries', 50.0, '2023-04-12', 2, 4);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Gasoline', 30.0, '2023-04-04', 3, 5);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Internet Bill', 50.0, '2023-04-01', 2, 3);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Dinner at Restaurant', 80.0, '2023-04-06', 1, 6);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Haircut', 25.0, '2023-04-13', 3, 1);
INSERT INTO finance.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Movie Ticket', 12.0, '2023-04-19', 2, 7);

/* Add 3 Income Transactions tb_transaction */
INSERT INTO finance.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Salary', 5000.0, 'R', '2023-04-07', 1, 12);
INSERT INTO finance.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Rental Income', 800.0, 'R', '2023-04-15', 2, 15);
INSERT INTO finance.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Investment Dividend', 150.0, 'R', '2023-03-05', 3, 14);

/*
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Moradia', 'Aluguel, hipoteca, condomínio, serviços públicos, reparos e manutenção da casa.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Transporte', 'Combustível, manutenção do veículo, seguro, impostos, transporte público.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Alimentação', 'Mercearia, restaurantes, cafeterias, lanches.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Entretenimento', 'Cinema, teatro, shows, esportes, hobbies.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Saúde', 'Seguro saúde, consultas médicas, medicamentos, tratamentos.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Vestuário', 'Roupas, sapatos, acessórios.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Educação', 'Mensalidades escolares, livros, cursos, treinamentos.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Dívidas', 'Pagamentos de empréstimos, cartões de crédito, juros.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Impostos', 'Imposto de renda, IPTU, IPVA, taxas.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Viagem', 'Passagens aéreas, hospedagem, transporte, alimentação.', 'D');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Outros', 'Gastos que não se encaixam em nenhuma outra categoria.', 'D');
*/

/*
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Ganhos Pessoais', 'Salário, comissões, bônus, prêmios, gorjetas e outras formas de remuneração que uma pessoa recebe em seu trabalho.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Ganhos Empresariais', 'Lucro líquido, receita, retorno sobre o investimento (ROI) e outras métricas financeiras relacionadas ao desempenho da empresa.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Ganhos de Investimento', 'Juros, dividendos, ganhos de capital e outras formas de renda geradas a partir de investimentos financeiros.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Ganhos Passivos', 'Renda de aluguel, royalties de propriedade intelectual, renda de publicidade online e outras formas de renda geradas sem a necessidade de trabalho ativo.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Ganhos de Jogo/Loteria', 'Ganhos provenientes de jogos de azar e loterias.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Outras Fontes de Renda', 'Heranças, doações, vendas de bens pessoais e outras formas de renda que não se encaixam em outras categorias.', 'R');
INSERT INTO finance.tb_category (name, description, type)
    VALUES ('Outros', 'Receitas que não se enquadram em nenhuma das categorias.', 'R');
*/